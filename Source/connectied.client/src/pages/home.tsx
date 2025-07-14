import { useEffect, useState } from "react"
import { useBreadcrumb } from "@/hooks/use-breadcrumb"
import type { GuestList } from "@/types"
import { client } from "@/api"
import { columns, DataTable } from "@/components/guest-list"

export default function GuestListPage() {
    const [guestList, setGuestList] = useState<GuestList[]>([])
    const [isLoading, setIsLoading] = useState(true)
    const { setItems: setBreadcrumbItems } = useBreadcrumb()

    useEffect(() => {
        const loadGuestLists = async () => {
            try {
                const data = await client.getGuestList()
                setGuestList(data)
            } catch (error) {
                console.error("Failed to fetch guest lists", error)
            } finally {
                setIsLoading(false)
            }
        }

        loadGuestLists()
    }, [])

    useEffect(() => {
        setBreadcrumbItems([
            { title: "Guest List" },
        ])
    }, [setBreadcrumbItems])

    if (isLoading) return <p>Loading guest lists...</p>

    return (
        <DataTable columns={columns} data={guestList} />
    )
}
