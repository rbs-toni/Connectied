interface BreadcrumbItem {
    title: string
    href?: string
}

interface GuestGroup {
    id: string
    name: string
}

interface Guest {
    id: string
    name: string
    email?: string
    phoneNumber?: string
    group?: GuestGroup
    event1Quota: number
    event2Quota: number
    event1Rsvp: number
    event2Rsvp: number
    event1Attend: number
    event2Attend: number
    event1Angpao: number
    event2Angpao: number
    event1Gift: number
    event2Gift: number
    event1Souvenir: number
    event2Souvenir: number
    notes?: string
}

interface CreateGuest {
    group?: string | null
    name?: string | null
}

interface GuestListConfiguration {
    columns?: string[]
    groups?: string[]
    includedGuests?: string[]
    excludedGuests?: string[]
}

interface GuestList {
    id?: string
    name?: string
    linkCode?: string
    configuration?: GuestListConfiguration
}

interface GuestListWithGuests {
    id?: string
    name?: string
    linkCode?: string
    configuration?: GuestListConfiguration
    guests?: Guest[]
}

interface GuestStats {
    event1Quota: number
    event2Quota: number
    quota: number

    event1Attendance: number
    event2Attendance: number
    attendance: number

    event1Angpao: number
    event2Angpao: number
    angpao: number

    event1Gift: number
    event2Gift: number
    gift: number

    event1Souvenir: number
    event2Souvenir: number
    souvenir: number
}

interface UpdateGuest {
    id: string | null
    name?: string | null
    group?: string | null
}

interface Search {
    fields: string[]
    keyword?: string
}

type FilterLogic = "and" | "or" | "xor"

type FilterOperator =
    | "contains"
    | "endswith"
    | "eq"
    | "gt"
    | "gte"
    | "lt"
    | "lte"
    | "neq"
    | "startswith"
interface Filter {
    field?: string
    operator?: string
    value?: any
    logic?: string
    filters?: Filter[]
}

interface PaginationFilter {
    page: number
    pageSize: number
    orderBy?: string[]
    keyword?: string
    advancedSearch?: Search
    advancedFilter?: Filter
}

interface PagedList<T> {
    items: T[]
    page: number
    pageSize: number
    totalCount: number
    totalPages: number
    hasPrevious: boolean
    hasNext: boolean
}

export type {
    BreadcrumbItem,
    Guest,
    GuestGroup,
    CreateGuest,
    UpdateGuest,
    GuestListConfiguration,
    GuestList,
    GuestListWithGuests,
    GuestStats,
    Search,
    FilterLogic,
    FilterOperator,
    Filter,
    PaginationFilter,
    PagedList
}