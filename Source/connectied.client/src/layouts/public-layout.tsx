import { Outlet } from "react-router-dom"

export default function PublicLayout() {
    return (
        <div className="flex flex-1 flex-col gap-4 p-4">
            <Outlet />
        </div>
    )
}
