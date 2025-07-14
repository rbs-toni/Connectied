import type {
    CreateGuest,
    UpdateGuest,
    PaginationFilter,
    GuestList,
    PagedList
} from "@/types"

const BASE_URL = "/api/guest-list"

function toQueryString(params: Record<string, any>): string {
    const query = new URLSearchParams()

    for (const key in params) {
        const value = params[key]

        if (Array.isArray(value)) {
            for (const item of value) {
                query.append(key, item)
            }
        } else if (value !== undefined && value !== null) {
            query.set(
                key,
                typeof value === "object" ? JSON.stringify(value) : String(value)
            )
        }
    }

    return query.toString()
}

async function handleJson<T>(res: Response): Promise<T> {
    if (!res.ok) {
        throw new Error(`Request failed with status ${res.status}`)
    }
    return res.json() as Promise<T>
}

export const client = {
    async getGuestList(): Promise<GuestList[]> {
        const res = await fetch(`${BASE_URL}`, {
            method: "GET"
        })
        return handleJson<GuestList[]>(res)
    },
    async searchGuestList(filter: PaginationFilter): Promise<PagedList<GuestList>> {
        const query = toQueryString(filter)
        const res = await fetch(`${BASE_URL}?${query}`, {
            method: "GET"
        })
        return handleJson<PagedList<GuestList>>(res)
    },

    async createGuest(body: CreateGuest): Promise<Response> {
        return fetch(BASE_URL, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(body)
        })
    },

    async updateGuest(id: string, body: UpdateGuest): Promise<Response> {
        return fetch(`${BASE_URL}/${id}`, {
            method: "PUT",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(body)
        })
    },

    async deleteGuest(id: string): Promise<Response> {
        return fetch(`${BASE_URL}/${id}`, {
            method: "DELETE"
        })
    }
}
