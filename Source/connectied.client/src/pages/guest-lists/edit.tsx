"use client"

import { useEffect, useState } from "react"
import { useNavigate, useParams } from "react-router-dom"
import { useForm } from "react-hook-form"
import { zodResolver } from "@hookform/resolvers/zod"
import { toast } from "sonner"
import { z } from "zod"

import { Button } from "@/components/ui/button"
import {
    Form,
    FormControl,
    FormDescription,
    FormField,
    FormItem,
    FormLabel,
    FormMessage,
} from "@/components/ui/form"
import { Input } from "@/components/ui/input"
import { Checkbox } from "@/components/ui/checkbox"
import { useBreadcrumb } from "@/hooks/use-breadcrumb"
import { client } from "@/api"
import type { Guest, GuestGroup, GuestList } from "@/types"
import { TypographyH1 } from "@/components/h1"

const AVAILABLE_COLUMNS = ["Name", "Group", "Email", "Phone"]

const FormSchema = z.object({
    name: z.string().min(1, "Name is required").max(50, {
        message: "Name must be at most 50 characters.",
    }),
    configuration: z.object({
        columns: z.array(z.string()).optional(),
        groups: z.array(z.string()).optional(),
        includedGuests: z.array(z.string()).optional(),
        excludedGuests: z.array(z.string()).optional(),
    }).optional(),
})

export default function EditGuestListPage() {
    const { id } = useParams()
    const { setItems: setBreadcrumbItems } = useBreadcrumb()
    const navigate = useNavigate()

    const [guestGroups, setGuestGroups] = useState<GuestGroup[]>([])
    const [guests, setGuests] = useState<Guest[]>([])
    const [groupSearch, setGroupSearch] = useState("")
    const [guestSearch, setGuestSearch] = useState("")
    const [isSubmitting, setIsSubmitting] = useState(false)

    const form = useForm<z.infer<typeof FormSchema>>({
        resolver: zodResolver(FormSchema),
        mode: "onChange",
        defaultValues: {
            name: "",
            configuration: {
                columns: AVAILABLE_COLUMNS,
                groups: [],
                includedGuests: [],
                excludedGuests: [],
            },
        },
    })

    useEffect(() => {
        setBreadcrumbItems([
            { href: "/guest-lists", title: "Guest Lists" },
            { title: "Edit" },
        ])

        client.getGuestGroups().then(setGuestGroups).catch(console.error)
        client.getGuests().then(setGuests).catch(console.error)

        if (id) {
            client.getGuestListById(id)
                .then(data => {
                    if (!data) throw new Error("Guest list not found")

                    form.reset({
                        name: data.name,
                        configuration: {
                            columns: data.configuration?.columns ?? [],
                            groups: data.configuration?.groups ?? [],
                            includedGuests: data.configuration?.includedGuests ?? [],
                            excludedGuests: data.configuration?.excludedGuests ?? [],
                        },
                    })
                })
                .catch(err => {
                    toast.error("Failed to load guest list")
                    console.error(err)
                    navigate("/guest-lists")
                })
        }
    }, [id])

    async function onSubmit(data: z.infer<typeof FormSchema>) {
        setIsSubmitting(true)

        try {
            const payload: Omit<GuestList, "id" | "linkCode"> = {
                name: data.name,
                configuration: {
                    columns: data.configuration?.columns || [],
                    groups: data.configuration?.groups || [],
                    includedGuests: data.configuration?.includedGuests || [],
                    excludedGuests: data.configuration?.excludedGuests || [],
                },
            }

            await client.updateGuestList(String(id), payload)

            toast.success("Guest List Updated")
            navigate(`/guest-lists/details/${id}`)
        } catch (error) {
            toast.error("Failed to update guest list")
            console.error(error)
        } finally {
            setIsSubmitting(false)
        }
    }

    function onCancel() {
        navigate(`/guest-lists/details/${id}`)
    }

    const filteredGroups = guestGroups.filter(group =>
        group.name.toLowerCase().includes(groupSearch.toLowerCase())
    )

    const filteredGuests = guests.filter(guest =>
        guest.name.toLowerCase().includes(guestSearch.toLowerCase())
    )

    return (
        <div className="max-w-4xl space-y-4">
            <TypographyH1>Edit Guest List</TypographyH1>
            <Form {...form}>
                <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-6">
                    <FormField
                        control={form.control}
                        name="name"
                        render={({ field }) => (
                            <FormItem>
                                <FormLabel>Name</FormLabel>
                                <FormControl>
                                    <Input placeholder="Enter guest list name" {...field} />
                                </FormControl>
                                <FormDescription>This is the name of the guest list.</FormDescription>
                                <FormMessage />
                            </FormItem>
                        )}
                    />

                    <FormField
                        control={form.control}
                        name="configuration.columns"
                        render={({ field }) => (
                            <FormItem>
                                <FormLabel>Columns</FormLabel>
                                <div className="flex flex-col gap-2">
                                    {AVAILABLE_COLUMNS.map((col) => {
                                        const isChecked = field.value?.includes(col) ?? false
                                        return (
                                            <div key={col} className="flex items-center gap-2">
                                                <Checkbox
                                                    checked={isChecked}
                                                    onCheckedChange={(checked) => {
                                                        if (checked) {
                                                            field.onChange([...(field.value || []), col])
                                                        } else {
                                                            field.onChange(field.value?.filter(v => v !== col))
                                                        }
                                                    }}
                                                />
                                                <span className="text-sm">{col}</span>
                                            </div>
                                        )
                                    })}
                                </div>
                                <FormDescription>Select which columns to show.</FormDescription>
                                <FormMessage />
                            </FormItem>
                        )}
                    />

                    <FormField
                        control={form.control}
                        name="configuration.groups"
                        render={({ field }) => (
                            <FormItem>
                                <FormLabel>Guest Groups</FormLabel>
                                <FormControl>
                                    <div className="flex items-center gap-2">
                                        <Input
                                            placeholder="Search groups"
                                            value={groupSearch}
                                            onChange={(e) => setGroupSearch(e.target.value)}
                                        />
                                        <Button
                                            type="button"
                                            variant="outline"
                                            size="sm"
                                            onClick={() => {
                                                const selected = form.getValues("configuration.groups") || []
                                                const filteredIds = filteredGroups.map((g) => g.id)
                                                const newSelection = [...new Set([...selected, ...filteredIds])]
                                                form.setValue("configuration.groups", newSelection)
                                            }}
                                        >
                                            Select All
                                        </Button>
                                        {(form.watch("configuration.groups") || []).some(id =>
                                            filteredGroups.some(g => g.id === id)
                                        ) && (
                                                <Button
                                                    type="button"
                                                    variant="outline"
                                                    size="sm"
                                                    onClick={() => {
                                                        const selected = form.getValues("configuration.groups") || []
                                                        const filteredIds = filteredGroups.map((g) => g.id)
                                                        const newSelection = selected.filter(id => !filteredIds.includes(id))
                                                        form.setValue("configuration.groups", newSelection)
                                                    }}
                                                >
                                                    Deselect
                                                </Button>
                                            )}
                                    </div>
                                </FormControl>

                                <div className="flex flex-col gap-2 mt-2 max-h-40 overflow-y-auto border rounded p-2">
                                    {filteredGroups.map((group) => {
                                        const isChecked = field.value?.includes(group.id) ?? false

                                        return (
                                            <FormItem
                                                key={group.id}
                                                className="flex items-center gap-2 space-y-0"
                                            >
                                                <FormControl>
                                                    <Checkbox
                                                        checked={isChecked}
                                                        onCheckedChange={(checked) => {
                                                            const newValue = checked
                                                                ? [...(field.value || []), group.id]
                                                                : (field.value || []).filter(id => id !== group.id)
                                                            field.onChange(newValue)
                                                        }}
                                                    />
                                                </FormControl>
                                                <FormLabel className="text-sm font-normal">
                                                    {group.name}
                                                </FormLabel>
                                            </FormItem>
                                        )
                                    })}
                                </div>
                                <FormDescription>Select groups to include in this guest list.</FormDescription>
                                <FormMessage />
                            </FormItem>
                        )}
                    />

                    <FormItem>
                        <FormLabel>Custom Guests</FormLabel>
                        <FormControl>
                            <div className="flex items-center gap-2">
                                <Input
                                    placeholder="Search guests"
                                    value={guestSearch}
                                    onChange={(e) => setGuestSearch(e.target.value)}
                                />
                            </div>
                        </FormControl>

                        <div className="flex flex-col gap-2 mt-2 max-h-64 overflow-y-auto border rounded p-2">
                            {[...filteredGuests]
                                .sort((a, b) => {
                                    const selectedGroups = form.getValues("configuration.groups") || []
                                    const includedGuests = form.getValues("configuration.includedGuests") || []
                                    const excludedGuests = form.getValues("configuration.excludedGuests") || []

                                    const aId = a.id, bId = b.id
                                    const aSelected = includedGuests.includes(aId) ||
                                        (a.group?.id && selectedGroups.includes(a.group.id) && !excludedGuests.includes(aId))
                                    const bSelected = includedGuests.includes(bId) ||
                                        (b.group?.id && selectedGroups.includes(b.group.id) && !excludedGuests.includes(bId))

                                    if (aSelected !== bSelected) return aSelected ? -1 : 1

                                    const aGroupName = a.group?.name || "~"
                                    const bGroupName = b.group?.name || "~"
                                    return aGroupName.localeCompare(bGroupName)
                                })
                                .map((guest) => {
                                    const guestId = guest.id
                                    const groupId = guest.group?.id
                                    const included = form.watch("configuration.includedGuests") || []
                                    const excluded = form.watch("configuration.excludedGuests") || []
                                    const selectedGroups = form.watch("configuration.groups") || []

                                    const isInGroup = groupId && selectedGroups.includes(groupId)
                                    const isIncluded = included.includes(guestId)
                                    const isExcluded = excluded.includes(guestId)

                                    const isChecked = isIncluded || (isInGroup && !isExcluded)

                                    return (
                                        <FormItem key={guestId} className="flex items-center gap-2 space-y-0">
                                            <FormControl>
                                                <Checkbox
                                                    checked={isChecked}
                                                    onCheckedChange={(checked) => {
                                                        const newIncluded = [...included]
                                                        const newExcluded = [...excluded]

                                                        if (checked) {
                                                            if (!isInGroup && !newIncluded.includes(guestId)) {
                                                                newIncluded.push(guestId)
                                                            }
                                                            const ix = newExcluded.indexOf(guestId)
                                                            if (ix !== -1) newExcluded.splice(ix, 1)
                                                        } else {
                                                            if (isInGroup && !newExcluded.includes(guestId)) {
                                                                newExcluded.push(guestId)
                                                            } else {
                                                                const ix = newIncluded.indexOf(guestId)
                                                                if (ix !== -1) newIncluded.splice(ix, 1)
                                                            }
                                                        }

                                                        form.setValue("configuration.includedGuests", newIncluded)
                                                        form.setValue("configuration.excludedGuests", newExcluded)
                                                    }}
                                                />
                                            </FormControl>
                                            <FormLabel className="text-sm font-normal">
                                                {guest.name}
                                                <span className="text-muted-foreground text-xs ml-2">
                                                    ({guest.group?.name ?? "No Group"})
                                                </span>
                                            </FormLabel>
                                        </FormItem>
                                    )
                                })}
                        </div>
                        <FormDescription>
                            Guests are selected automatically if their group is selected. You can override manually.
                        </FormDescription>
                    </FormItem>

                    <div className="flex justify-end gap-4">
                        <Button type="button" variant="outline" onClick={onCancel}>
                            Cancel
                        </Button>
                        <Button
                            type="submit"
                            disabled={!form.formState.isValid || !form.formState.isDirty || isSubmitting}
                        >
                            {isSubmitting ? "Saving..." : "Save Changes"}
                        </Button>
                    </div>
                </form>
            </Form>
        </div>
    )
}
