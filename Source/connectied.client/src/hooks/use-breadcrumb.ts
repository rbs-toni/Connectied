import { useContext } from "react"
import { BreadcrumbContext } from "../components/breadcrumb-context"

export function useBreadcrumb() {
    const context = useContext(BreadcrumbContext)
    if (!context) {
        throw new Error("useBreadcrumb must be used within a BreadcrumbProvider")
    }
    return context
}