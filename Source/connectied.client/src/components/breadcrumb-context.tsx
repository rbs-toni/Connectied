import { createContext, useState, type ReactNode } from "react"
import type { BreadcrumbItem } from "../types/breadcrumb-item"

type BreadcrumbContextType = {
    items: BreadcrumbItem[]
    setItems: (items: BreadcrumbItem[]) => void
}

export const BreadcrumbContext = createContext<BreadcrumbContextType | undefined>(undefined)

export function BreadcrumbProvider({ children }: { children: ReactNode }) {
    const [items, setItems] = useState<BreadcrumbItem[]>([])

    return (
        <BreadcrumbContext.Provider value={{ items, setItems }}>
            {children}
        </BreadcrumbContext.Provider>
    )
}