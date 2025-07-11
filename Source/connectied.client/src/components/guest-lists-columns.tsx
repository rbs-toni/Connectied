"use client"

import type { ColumnDef } from "@tanstack/react-table"
import type { GuestList } from "../types/guest-list"

export const columns: ColumnDef<GuestList>[] = [
    {
        accessorKey: "id",
        header: "Id",
    },
    {
        accessorKey: "name",
        header: "Name",
    },
    {
        accessorKey: "group",
        header: "Group",
    },
    {
        accessorKey: "event1Quota",
        header: "Event 1 Quota",
    },
    {
        accessorKey: "event2Quota",
        header: "Event 2 Quota",
    },
    {
        accessorKey: "event1Rsvp",
        header: "Event 1 RSVP",
    },
    {
        accessorKey: "event2Rsvp",
        header: "Event 2 RSVP",
    },
    {
        accessorKey: "event1Attend",
        header: "Event 1 Attend",
    },
    {
        accessorKey: "event2Attend",
        header: "Event 2 Attend",
    },
    {
        accessorKey: "event2AngpaoCount",
        header: "Event 2 Angpao Count",
    },
    {
        accessorKey: "event2GiftCount",
        header: "Event 2 Gift Count",
    },
    {
        accessorKey: "event2Souvenir",
        header: "Event 2 Souvenir",
    },
    {
        accessorKey: "notes",
        header: "Notes",
    },
]
