"use client"

import type {
    ColumnDef
} from "@tanstack/react-table"
import type {
    Guest
} from "@/types"
import { Checkbox } from "@/components/ui/checkbox"
import {
    DropdownMenu,
    DropdownMenuContent,
    DropdownMenuItem,
    DropdownMenuLabel,
    DropdownMenuSeparator,
    DropdownMenuTrigger
} from "@/components/ui/dropdown-menu"
import { Button } from "@/components//ui/button"
import { MoreHorizontal } from "lucide-react"

export const columns: ColumnDef<Guest>[] = [
    {
        id: "select",
        header: ({ table }) => (
            <div className="flex items-center justify-center">
                <Checkbox
                    checked={
                        table.getIsAllPageRowsSelected() ||
                        (table.getIsSomePageRowsSelected() && "indeterminate")
                    }
                    onCheckedChange={(value) => table.toggleAllPageRowsSelected(!!value)}
                    aria-label="Select all"
                />
            </div>
        ),
        cell: ({ row }) => (
            <Checkbox
                checked={row.getIsSelected()}
                onCheckedChange={(value) => row.toggleSelected(!!value)}
                aria-label="Select row"
            />
        ),
        enableSorting: false,
        enableHiding: false,
    },
    {
        accessorKey: "name",
        header: () => <div className="text-left">Name</div>,
        cell: ({ row }) => {
            const value: string = row.getValue("name")
            return <div className="text-left font-medium">{value}</div>
        },
    },
    {
        accessorKey: "group",
        header: () => <div className="text-left">Group</div>,
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
    {
        id: "actions",
        cell: ({ row }) => {
            const id: string = row.getValue('id')

            return (
                <DropdownMenu>
                    <DropdownMenuTrigger asChild>
                        <Button variant="ghost" className="h-8 w-8 p-0">
                            <span className="sr-only">Open menu</span>
                            <MoreHorizontal className="h-4 w-4" />
                        </Button>
                    </DropdownMenuTrigger>
                    <DropdownMenuContent align="end">
                        <DropdownMenuLabel>Actions</DropdownMenuLabel>
                        <DropdownMenuSeparator />
                        <DropdownMenuItem>View</DropdownMenuItem>
                        <DropdownMenuItem>Edit</DropdownMenuItem>
                        <DropdownMenuItem>Update</DropdownMenuItem>
                        <DropdownMenuItem>Delete</DropdownMenuItem>
                    </DropdownMenuContent>
                </DropdownMenu>
            )
        },
    },
]
