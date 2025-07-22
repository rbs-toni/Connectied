import type {
    CreateGuest,
    UpdateGuest,
    PaginationFilter,
    GuestList,
    PagedList,
    GuestStats,
    Guest,
    GuestGroup,
    GuestListWithGuests
} from "@/types"

const BASE_URL = "/api"

const endpoints = {
    dashboard: `${BASE_URL}/dashboard`,
    guestLists: `${BASE_URL}/guest-lists`,
    guests: `${BASE_URL}/guests`,
    guestGroups: `${BASE_URL}/guest-groups`
}

function toQueryString(params: Record<string, any>): string {
    const query = new URLSearchParams()

    for (const key in params) {
        const value = params[key]
        if (Array.isArray(value)) {
            value.forEach(item => query.append(key, item))
        } else if (value !== undefined && value !== null) {
            query.set(key, typeof value === "object" ? JSON.stringify(value) : String(value))
        }
    }

    return query.toString()
}

async function handleJson<T>(res: Response): Promise<T> {
    if (!res.ok) {
        throw new Error(`Request failed with status ${res.status}`)
    }
    return res.json()
}

async function get<T>(url: string): Promise<T> {
    const res = await fetch(url, { method: "GET" })
    return handleJson(res)
}

async function post<T>(url: string, body: unknown): Promise<T> {
    const res = await fetch(url, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(body)
    })
    return handleJson(res)
}

async function put<T>(url: string, body: unknown): Promise<T> {
    const res = await fetch(url, {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(body)
    })
    return handleJson(res)
}

function del(url: string): Promise<Response> {
    return fetch(url, { method: "DELETE" })
}

export const client = {
    // 📊 Dashboard
    getGuestStats(): Promise<GuestStats> {
        return get<GuestStats>(`${endpoints.dashboard}`)
    },

    // 👥 Guest Groups
    getGuestGroups(): Promise<GuestGroup[]> {
        return get<GuestGroup[]>(endpoints.guestGroups)
    },

    // 📋 Guest List
    getGuestLists(): Promise<GuestList[]> {
        return get<GuestList[]>(endpoints.guestLists)
    },

    searchGuestLists(filter: PaginationFilter): Promise<PagedList<GuestList>> {
        const query = toQueryString(filter)
        return get<PagedList<GuestList>>(`${endpoints.guestLists}?${query}`)
    },

    getGuestListById(id: string): Promise<GuestList> {
        return get<GuestList>(`${endpoints.guestLists}/${id}`)
    },

    getGuestListWithGuests(id: string): Promise<GuestListWithGuests> {
        return get<GuestListWithGuests>(`${endpoints.guestLists}/${id}`)
    },

    getGuestListByCode(code: string): Promise<GuestList> {
        return get<GuestList>(`${endpoints.guestLists}/code/${code}`)
    },

    createGuestList(body: GuestList): Promise<GuestList> {
        return post<GuestList>(endpoints.guestLists, body)
    },

    updateGuestList(id: string, body: GuestList): Promise<GuestList> {
        return put<GuestList>(`${endpoints.guestLists}/${id}`, body)
    },

    deleteGuestList(id: string): Promise<Response> {
        return del(`${endpoints.guestLists}/${id}`)
    },

    // 🙋‍♂️ Guest
    getGuests(): Promise<Guest[]> {
        return get<Guest[]>(endpoints.guests)
    },

    createGuest(body: CreateGuest): Promise<Guest> {
        return post<Guest>(endpoints.guests, body)
    },

    updateGuest(id: string, body: UpdateGuest): Promise<Guest> {
        return put<Guest>(`${endpoints.guests}/${id}`, body)
    },

    deleteGuest(id: string): Promise<Response> {
        return del(`${endpoints.guests}/${id}`)
    }
}
