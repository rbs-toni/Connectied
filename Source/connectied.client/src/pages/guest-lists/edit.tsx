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
    const { setItems: setBreadcrumbItems } = useBreadcrumb()
    const navigate = useNavigate()
    const { id } = useParams()
    const [guestGroups, setGuestGroups] = useState<GuestGroup[]>([])
    const [guests, setGuests] = useState<Guest[]>([])
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

        if (!id) return

        Promise.all([
            client.getGuestListById(id),
            client.getGuestGroups(),
            client.getGuests(),
        ])
            .then(([guestList, groups, allGuests]) => {
                form.reset({
                    name: guestList.name,
                    configuration: {
                        columns: guestList.configuration?.columns || [],
                        groups: guestList.configuration?.groups || [],
                        includedGuests: guestList.configuration?.includedGuests || [],
                        excludedGuests: guestList.configuration?.excludedGuests || [],
                    },
                })
                setGuestGroups(groups)
                setGuests(allGuests)
            })
            .catch(console.error)
    }, [id, form, setBreadcrumbItems])

    async function onSubmit(data: z.infer<typeof FormSchema>) {
        if (!id) return
        setIsSubmitting(true)

        try {
            const payload: GuestList = {
                id,
                name: data.name,
                linkCode: "", // Use existing one or regenerate if required
                configuration: {
                    columns: data.configuration?.columns || [],
                    groups: data.configuration?.groups || [],
                    includedGuests: data.configuration?.includedGuests || [],
                    excludedGuests: data.configuration?.excludedGuests || [],
                },
            }

            await client.updateGuestList(id, payload)

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
        navigate("/guest-lists")
    }

    return (
        <div className="max-w-4xl space-y-6">
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
                                    <Input placeholder="e.g. VIP Guests" {...field} />
                                </FormControl>
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
                                <div className="flex flex-wrap gap-2">
                                    {AVAILABLE_COLUMNS.map(col => (
                                        <label key={col} className="flex items-center gap-2">
                                            <Checkbox
                                                checked={field.value?.includes(col)}
                                                onCheckedChange={checked => {
                                                    const value = field.value || []
                                                    if (checked) {
                                                        field.onChange([...value, col])
                                                    } else {
                                                        field.onChange(value.filter(v => v !== col))
                                                    }
                                                }}
                                            />
                                            {col}
                                        </label>
                                    ))}
                                </div>
                                <FormMessage />
                            </FormItem>
                        )}
                    />

                    <FormField
                        control={form.control}
                        name="configuration.groups"
                        render={({ field }) => (
                            <FormItem>
                                <FormLabel>Groups</FormLabel>
                                <div className="flex flex-col gap-2 max-h-60 overflow-y-auto border p-2 rounded">
                                    {guestGroups.map(group => (
                                        <label key={group.id} className="flex items-center gap-2">
                                            <Checkbox
                                                checked={field.value?.includes(group.id)}
                                                onCheckedChange={checked => {
                                                    const value = field.value || []
                                                    if (checked) {
                                                        field.onChange([...value, group.id])
                                                    } else {
                                                        field.onChange(value.filter(v => v !== group.id))
                                                    }
                                                }}
                                            />
                                            {group.name}
                                        </label>
                                    ))}
                                </div>
                                <FormMessage />
                            </FormItem>
                        )}
                    />

                    <FormField
                        control={form.control}
                        name="configuration.includedGuests"
                        render={({ field }) => (
                            <FormItem>
                                <FormLabel>Included Guests</FormLabel>
                                <div className="flex flex-col gap-2 max-h-60 overflow-y-auto border p-2 rounded">
                                    {guests.map(guest => (
                                        <label key={guest.id} className="flex items-center gap-2">
                                            <Checkbox
                                                checked={field.value?.includes(guest.id)}
                                                onCheckedChange={checked => {
                                                    const value = field.value || []
                                                    if (checked) {
                                                        field.onChange([...value, guest.id])
                                                    } else {
                                                        field.onChange(value.filter(v => v !== guest.id))
                                                    }
                                                }}
                                            />
                                            {guest.name}
                                        </label>
                                    ))}
                                </div>
                                <FormMessage />
                            </FormItem>
                        )}
                    />

                    <FormField
                        control={form.control}
                        name="configuration.excludedGuests"
                        render={({ field }) => (
                            <FormItem>
                                <FormLabel>Excluded Guests</FormLabel>
                                <div className="flex flex-col gap-2 max-h-60 overflow-y-auto border p-2 rounded">
                                    {guests.map(guest => (
                                        <label key={guest.id} className="flex items-center gap-2">
                                            <Checkbox
                                                checked={field.value?.includes(guest.id)}
                                                onCheckedChange={checked => {
                                                    const value = field.value || []
                                                    if (checked) {
                                                        field.onChange([...value, guest.id])
                                                    } else {
                                                        field.onChange(value.filter(v => v !== guest.id))
                                                    }
                                                }}
                                            />
                                            {guest.name}
                                        </label>
                                    ))}
                                </div>
                                <FormMessage />
                            </FormItem>
                        )}
                    />

                    <div className="flex justify-end gap-4">
                        <Button type="button" variant="outline" onClick={onCancel}>
                            Cancel
                        </Button>
                        <Button type="submit" disabled={!form.formState.isValid || isSubmitting}>
                            {isSubmitting ? "Updating..." : "Update"}
                        </Button>
                    </div>
                </form>
            </Form>
        </div>
    )
}
