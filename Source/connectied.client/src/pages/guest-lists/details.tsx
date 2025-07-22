"use client"

import { useEffect, useMemo, useState } from "react"
import { useParams, useNavigate, Link } from "react-router-dom"
import { useBreadcrumb } from "@/hooks/use-breadcrumb"
import { client } from "@/api"
import type { GuestListWithGuests, Guest } from "@/types"
import { Button } from "@/components/ui/button"
import { Card, CardContent } from "@/components/ui/card"
import {
    Table,
    TableBody,
    TableCell,
    TableHead,
    TableHeader,
    TableRow,
} from "@/components/ui/table"

const AVAILABLE_COLUMNS = ["Name", "Group", "Email", "Phone"]

export default function DetailsGuestListPage() {
    const { id } = useParams<{ id: string }>()
    const navigate = useNavigate()
    const { setItems: setBreadcrumbItems } = useBreadcrumb()

    const [guestList, setGuestList] = useState<GuestListWithGuests | null>(null)
    const [isLoading, setIsLoading] = useState(true)

    useEffect(() => {
        if (!id) return
        const fetchData = async () => {
            try {
                const data = await client.getGuestListWithGuests(id)
                setGuestList(data)
            } catch (err) {
                console.error("Failed to load guest list", err)
            } finally {
                setIsLoading(false)
            }
        }
        fetchData()
    }, [id])

    useEffect(() => {
        setBreadcrumbItems([
            { title: "Guest List", href: "/guest-lists" },
            { title: guestList?.name || "..." },
        ])
    }, [guestList?.name, setBreadcrumbItems])

    const filteredGuests = useMemo(() => {
        if (!guestList) return []

        const guests = guestList.guests || []
        const included = new Set(guestList.configuration?.includedGuests || [])
        const excluded = new Set(guestList.configuration?.excludedGuests || [])
        const selectedGroups = new Set(guestList.configuration?.groups || [])

        return guests.filter(g => {
            if (excluded.has(g.id)) return false
            if (included.size > 0) return included.has(g.id)
            if (selectedGroups.size > 0) return g.group && selectedGroups.has(g.group.id)
            return true
        })
    }, [guestList])

    const selectedColumns = useMemo(() => {
        return guestList?.configuration?.columns || []
    }, [guestList])

    const renderGuestColumn = (guest: Guest, column: string) => {
        switch (column) {
            case "Name":
                return guest.name
            case "Group":
                return guest.group?.name || "-"
            case "Email":
                return guest.email || "-"
            case "Phone":
                return guest.phoneNumber || "-"
            default:
                return ""
        }
    }

    if (isLoading || !guestList) return <p className="text-sm text-muted-foreground">Loading guest list...</p>

    return (
        <div className="space-y-6">
            <div className="flex flex-wrap items-center justify-between gap-4">
                <div>
                    <h1 className="text-2xl font-semibold">{guestList.name}</h1>
                    <p className="text-muted-foreground text-sm">{guestList.id}</p>
                </div>
                <div className="flex flex-wrap gap-2">
                    <Button asChild>
                        <Link to={`/guest-lists/guests/${guestList.linkCode}`}>Open Public View</Link>
                    </Button>
                    <Button onClick={() => navigate(`/guest-lists/edit/${guestList.id}`)}>Edit</Button>
                    <Button
                        variant="destructive"
                        onClick={() => navigate(`/guest-lists/delete/${guestList.id}`)}
                    >
                        Delete
                    </Button>
                </div>
            </div>

            <Card>
                <CardContent className="pt-4 grid gap-3 text-sm">
                    <div>
                        <span className="font-medium">Columns:</span>{" "}
                        {selectedColumns.length ? selectedColumns.join(", ") : <span className="text-muted-foreground">-</span>}
                    </div>
                    <div>
                        <span className="font-medium">Groups:</span>{" "}
                        {guestList.configuration?.groups?.length
                            ? guestList.configuration.groups.join(", ")
                            : <span className="text-muted-foreground">-</span>}
                    </div>
                    <div>
                        <span className="font-medium">Included Guests:</span>{" "}
                        {guestList.configuration?.includedGuests?.length || 0}
                    </div>
                    <div>
                        <span className="font-medium">Excluded Guests:</span>{" "}
                        {guestList.configuration?.excludedGuests?.length || 0}
                    </div>
                </CardContent>
            </Card>

            <div>
                <h2 className="text-lg font-semibold">Guests</h2>
                <Card className="mt-2">
                    <CardContent className="pt-4 overflow-x-auto">
                        {filteredGuests.length === 0 ? (
                            <p className="text-muted-foreground">No guests matched this configuration.</p>
                        ) : (
                            <Table>
                                <TableHeader>
                                    <TableRow>
                                        {selectedColumns.map((col) => (
                                            <TableHead key={col}>{col}</TableHead>
                                        ))}
                                    </TableRow>
                                </TableHeader>
                                <TableBody>
                                    {filteredGuests.map((guest) => (
                                        <TableRow key={guest.id}>
                                            {selectedColumns.map((col) => (
                                                <TableCell key={col}>
                                                    {renderGuestColumn(guest, col)}
                                                </TableCell>
                                            ))}
                                        </TableRow>
                                    ))}
                                </TableBody>
                            </Table>
                        )}
                    </CardContent>
                </Card>
            </div>
        </div>
    )
}
