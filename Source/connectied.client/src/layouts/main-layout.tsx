import { Outlet } from "react-router-dom";
import { AppSidebar } from "@/components/app-sidebar";
import { SidebarProvider, SidebarInset } from "@/components/ui/sidebar";
import { AppHeader } from "@/components/app-header";
import { BreadcrumbProvider } from "../components/breadcrumb-context";

export default function MainLayout() {
    return (
        <BreadcrumbProvider>
            <SidebarProvider>
                <AppSidebar />
                <SidebarInset>
                    <AppHeader />
                    <div className="flex flex-1 flex-col gap-4 p-4">
                        <Outlet /> {/* This is where child routes will be rendered */}
                    </div>
                </SidebarInset>
            </SidebarProvider>
        </BreadcrumbProvider>
    );
}
