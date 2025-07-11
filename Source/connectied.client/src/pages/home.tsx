import { useEffect, useState } from "react"
import type { GuestList } from "../types/guest-list"
import { columns } from "../components/guest-lists-columns"
import { DataTable } from "../components/guest-lists-data-table"
import { useBreadcrumb } from "../hooks/use-breadcrumb"

export default function GuestListPage() {
    const [guestLists, setGuestLists] = useState<GuestList[]>([])
    const [isLoading, setIsLoading] = useState(true)
    const { setItems: setBreadcrumbItems } = useBreadcrumb()

    useEffect(() => {
        const loadGuestLists = async () => {
            try {
                const response = await fetch("/api/guest-lists")
                const data = await response.json()
                setGuestLists(data)
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
        <div className="container mx-auto py-10">
            <DataTable columns={columns} data={guestLists} />
        </div>
    )
}
