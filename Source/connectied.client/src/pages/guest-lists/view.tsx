import { useEffect, useState, useCallback } from "react"
import { useBreadcrumb } from "@/hooks/use-breadcrumb"
import type { GuestList } from "@/types"
import { client } from "@/api"
import { columns, DataTable } from "@/components/guest-lists"
import { useGuestListLiveUpdate } from "@/hooks/use-guest-list-live-update"

export default function ViewGuestListPage() {
    const [guestLists, setGuestLists] = useState<GuestList[]>([])
    const [isLoading, setIsLoading] = useState(true)
    const { setItems: setBreadcrumbItems } = useBreadcrumb()

    const fetchData = useCallback(async () => {
        setIsLoading(true)
        try {
            const data = await client.getGuestLists()
            setGuestLists(data)
        } catch (err) {
            console.error("Failed to fetch guest lists", err)
        } finally {
            setIsLoading(false)
        }
    }, [])

    useEffect(() => {
        fetchData()
        setBreadcrumbItems([{ title: "Guest List" }])
        return () => setBreadcrumbItems([])
    }, [fetchData, setBreadcrumbItems])

    useGuestListLiveUpdate({
        onCreated: fetchData,
        onUpdated: fetchData,
        onDeleted: fetchData,
    })

    if (isLoading) return <p>Loading guest lists...</p>

    return <DataTable columns={columns} data={guestLists} />
}
