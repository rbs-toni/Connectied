"use client"

import { useEffect, useMemo, useState } from "react"
import { useParams, useNavigate } from "react-router-dom"
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

    if (isLoading || !guestList) return <p>Loading guest list...</p>

    return (
        <div className="space-y-6">
            <div className="flex justify-between items-center">
                <h1 className="text-xl font-semibold">{guestList.name}</h1>
                <div className="space-x-2">
                    <Button onClick={() => navigate(`/guest-lists/edit/${guestList.id}`)}>Edit</Button>
                    <Button variant="destructive" onClick={() => navigate(`/guest-lists/delete/${guestList.id}`)}>Delete</Button>
                </div>
            </div>

            <Card>
                <CardContent className="pt-4 space-y-2">
                    <div><strong>Columns:</strong> {selectedColumns.join(", ") || "-"}</div>
                    <div><strong>Groups:</strong> {guestList.configuration?.groups?.length ? guestList.configuration.groups.join(", ") : "-"}</div>
                    <div><strong>Included Guests:</strong> {guestList.configuration?.includedGuests?.length || 0}</div>
                    <div><strong>Excluded Guests:</strong> {guestList.configuration?.excludedGuests?.length || 0}</div>
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
                                                <TableCell key={col}>{renderGuestColumn(guest, col)}</TableCell>
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
