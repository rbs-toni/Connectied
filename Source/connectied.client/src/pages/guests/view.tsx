import {
    useEffect,
    useState
} from "react"
import {
    useBreadcrumb
} from "@/hooks/use-breadcrumb"
import type {
    Guest,
} from "@/types"
import { client } from "@/api"
import { columns, DataTable } from "@/components/guests"

export default function ViewGuestPage() {
    const [guestList, setGuestList] = useState<Guest[]>([])
    const [isLoading, setIsLoading] = useState(true)
    const { setItems: setBreadcrumbItems } = useBreadcrumb()

    useEffect(() => {
        const loadGuestLists = async () => {
            try {
                const data = await client.getGuests()
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
            { title: "Guest" },
        ])
    }, [setBreadcrumbItems])

    if (isLoading) return <p>Loading guest lists...</p>

    return (
        <DataTable columns={columns} data={guestList} />
    )
}
