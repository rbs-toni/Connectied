import {
    BrowserRouter as Router,
    Routes,
    Route
} from "react-router-dom"
import MainLayout from "@/layouts/main-layout"
import PublicLayout from "@/layouts/public-layout"
import DashboardPage from "@/pages/dashboard"
import ViewGuestListPage from "@/pages/guest-lists/view"
import CreateGuestListPage from "@/pages/guest-lists/create"
import EditGuestListPage from "@/pages/guest-lists/edit"
import DetailsGuestListPage from "@/pages/guest-lists/details"
import DeleteGuestListPage from "@/pages/guest-lists/delete"
import GuestListGuestsPage from "@/pages/guest-lists/guests"

export default function App() {
    return (
        <Router>
            <Routes>
                <Route element={<MainLayout />}>
                    <Route path="/" element={<DashboardPage />} />
                    <Route path="/guest-lists" element={<ViewGuestListPage />} />
                    <Route path="/guest-lists/create" element={<CreateGuestListPage />} />
                    <Route path="/guest-lists/details/:id" element={<DetailsGuestListPage />} />
                    <Route path="/guest-lists/edit/:id" element={<EditGuestListPage />} />
                    <Route path="/guest-lists/delete/:id" element={<DeleteGuestListPage />} />
                </Route>
                <Route element={<PublicLayout />}>
                    <Route path="/guest-lists/:code/guests/" element={<GuestListGuestsPage />} />
                </Route>
            </Routes>
        </Router>
    )
}