﻿"use client"

import { useEffect, useState } from "react"
import { useParams } from "react-router-dom"
import { client } from "@/api"
import type { GuestListWithGuests } from "@/types"
import {
    Table,
    TableBody,
    TableCell,
    TableHead,
    TableHeader,
    TableRow,
} from "@/components/ui/table"

import { useGuestLiveUpdate } from "@/hooks/use-guest-live-update"
import { useGuestListLiveUpdate } from "@/hooks/use-guest-list-live-update"

const columnKeyMap: Record<string, string> = {
    Name: "name",
    Group: "group.name",
    Email: "email",
    Phone: "phoneNumber",
}

function getValue(obj: any, path: string): string {
    return path.split(".").reduce((o, key) => o?.[key], obj) ?? "-"
}

export default function GuestListGuestsPage() {
    const { code } = useParams()
    const [guestList, setGuestList] = useState<GuestListWithGuests | null>(null)

    // Initial load
    useEffect(() => {
        if (!code) return
        client.getGuestListWithGuestsByCode(code).then(setGuestList)
    }, [code])

    // Refresh only guests, don't reload guest list metadata
    const refreshGuestsOnly = async () => {
        if (!code) return
        const fresh = await client.getGuestListWithGuestsByCode(code)
        setGuestList((prev) =>
            prev ? { ...prev, guests: fresh.guests } : fresh
        )
    }

    useGuestLiveUpdate({
        onCreated: refreshGuestsOnly,
        onUpdated: refreshGuestsOnly,
        onDeleted: refreshGuestsOnly,
    })

    useGuestListLiveUpdate({
        onUpdated: async () => {
            if (!code) return
            const fresh = await client.getGuestListWithGuestsByCode(code)
            setGuestList(fresh)
        },
    })

    if (!guestList) return <p className="p-4">Loading...</p>

    const columns = guestList.configuration?.columns ?? []

    return (
        <div className="p-4 space-y-4">
            <h1 className="text-2xl font-bold">{guestList.name}</h1>
            <Table>
                <TableHeader>
                    <TableRow>
                        {columns.map((col) => (
                            <TableHead key={col}>{col}</TableHead>
                        ))}
                    </TableRow>
                </TableHeader>
                <TableBody>
                    {guestList.guests?.map((guest) => (
                        <TableRow key={guest.id}>
                            {columns.map((col) => (
                                <TableCell key={col}>
                                    {getValue(guest, columnKeyMap[col])}
                                </TableCell>
                            ))}
                        </TableRow>
                    ))}
                </TableBody>
            </Table>
        </div>
    )
}
