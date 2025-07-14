interface BreadcrumbItem {
    title: string
    href?: string
}
interface CreateGuest {
    group?: string | null
    name?: string | null
}
interface GuestList {
    id: string
    name: string
    group?: string
    event1Quota: number
    event2Quota: number
    event1Rsvp: number
    event2Rsvp: number
    event1Attend: number
    event2Attend: number
    event2AngpaoCount: number
    event2GiftCount: number
    event2Souvenir: number
    notes?: string
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
    GuestList,
    CreateGuest,
    UpdateGuest,
    Search,
    FilterLogic,
    FilterOperator,
    Filter,
    PaginationFilter,
    PagedList
}

