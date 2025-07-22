"use client"

import { useNavigate, useParams } from "react-router-dom"
import { useEffect } from "react"
import { client } from "@/api"

export default function DeleteGuestListPage() {
    const { id } = useParams()
    const navigate = useNavigate()

    useEffect(() => {
        if (!id) return
        client
            .deleteGuestList(id)
            .then(() => navigate("/guest-lists"))
            .catch(console.error)
    }, [id])

    return <p>Deleting guest list...</p>
}