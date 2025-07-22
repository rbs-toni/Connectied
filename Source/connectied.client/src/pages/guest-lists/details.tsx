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
import { toast } from "sonner"
import { CopyIcon } from "lucide-react"
import { Badge } from "../../components/ui/badge"
type PublicLinkTextProps = {
    linkCode?: string
}
const AVAILABLE_COLUMNS = ["Name", "Group", "Email", "Phone"]
function PublicLinkText({ linkCode }: PublicLinkTextProps) {
    const path = `/guest-lists/${linkCode}/guests`
    const fullUrl = `${window.location.origin}${path}`

    const handleCopy = () => {
        navigator.clipboard.writeText(fullUrl)
        toast.success("Public link copied")
    }


    return (
        <div className="flex items-center gap-2">
            <a
                href={fullUrl}
                target="_blank"
                rel="noopener noreferrer"
                className="text-sm text-blue-600 hover:underline break-all"
            >
                {fullUrl}
            </a>
            <Button
                size="icon"
                variant="ghost"
                onClick={handleCopy}
                className="shrink-0"
            >
                <CopyIcon className="w-4 h-4" />
            </Button>
        </div>
    )
}
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
                    <PublicLinkText linkCode={guestList.linkCode} />
                </div>
                <div className="flex flex-wrap gap-2">
                    <Button asChild>
                        <Link to={`/guest-lists/${guestList.linkCode}/guests`}>Open Public View</Link>
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
                <CardContent className="text-sm">
                    <div className="grid gap-y-3 gap-x-4 grid-cols-[150px_1fr]">
                        <div className="text-muted-foreground font-medium self-start pt-1">Columns:</div>
                        <div className="flex flex-wrap gap-1.5">
                            {selectedColumns.length ? (
                                selectedColumns.map(col => (
                                    <Badge key={col} variant="secondary">
                                        {col}
                                    </Badge>
                                ))
                            ) : (
                                <Badge variant="outline">-</Badge>
                            )}
                        </div>

                        <div className="text-muted-foreground font-medium self-start pt-1">Groups:</div>
                        <div className="flex flex-wrap gap-1.5">
                            {guestList.configuration?.groups?.length ? (
                                guestList.configuration.groups.map(group => (
                                    <Badge key={group} variant="secondary">
                                        {group}
                                    </Badge>
                                ))
                            ) : (
                                <Badge variant="outline">-</Badge>
                            )}
                        </div>

                        <div className="text-muted-foreground font-medium self-start pt-1">Included Guests:</div>
                        <div className="flex flex-wrap gap-1.5">
                            {guestList.configuration?.includedGuests?.length ? (
                                guestList.configuration.includedGuests.map(id => (
                                    <Badge key={id} variant="secondary">
                                        {id}
                                    </Badge>
                                ))
                            ) : (
                                <Badge variant="outline">-</Badge>
                            )}
                        </div>

                        <div className="text-muted-foreground font-medium self-start pt-1">Excluded Guests:</div>
                        <div className="flex flex-wrap gap-1.5">
                            {guestList.configuration?.excludedGuests?.length ? (
                                guestList.configuration.excludedGuests.map(id => (
                                    <Badge key={id} variant="secondary">
                                        {id}
                                    </Badge>
                                ))
                            ) : (
                                <Badge variant="outline">-</Badge>
                            )}
                        </div>
                    </div>
                </CardContent>
            </Card>

            <div>
                <h2 className="text-lg font-semibold">Guests</h2>
                <Card className="mt-2">
                    <CardContent className="overflow-x-auto">
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
