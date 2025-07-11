import {
    SidebarTrigger,
} from "@/components/ui/sidebar"
import {
    Breadcrumb,
    BreadcrumbList,
    BreadcrumbItem,
    BreadcrumbLink,
    BreadcrumbSeparator,
    BreadcrumbPage
} from "@/components/ui/breadcrumb"
import { Separator } from "@/components/ui/separator"
import { useBreadcrumb } from "../hooks/use-breadcrumb"

export function AppHeader() {
    const { items } = useBreadcrumb()

    return (
        <header className="flex h-16 shrink-0 items-center gap-2 border-b">
            <div className="flex items-center gap-2 px-3">
                <SidebarTrigger />
                <Separator orientation="vertical" className="mr-2 h-4" />
                <Breadcrumb>
                    <BreadcrumbList>
                        {items.map((item, i) => (
                            <BreadcrumbItem key={i} className="hidden md:block">
                                {item.href ? (
                                    <>
                                        <BreadcrumbLink href={item.href}>{item.title}</BreadcrumbLink>
                                        {i < items.length - 1 && <BreadcrumbSeparator />}
                                    </>
                                ) : (
                                    <BreadcrumbPage>{item.title}</BreadcrumbPage>
                                )}
                            </BreadcrumbItem>
                        ))}
                    </BreadcrumbList>
                </Breadcrumb>
            </div>
        </header>
    )
}