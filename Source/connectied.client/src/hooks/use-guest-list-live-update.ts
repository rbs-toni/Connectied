import { useEffect, useRef } from "react"
import { HubConnectionBuilder, type HubConnection } from "@microsoft/signalr"

const HUB_URL = "/hubs/guest-list"

export function useGuestListLiveUpdate(handlers: {
    onCreated?: (id: string) => void
    onUpdated?: (id: string) => void
    onDeleted?: (id: string) => void
}) {
    const connectionRef = useRef<HubConnection | null>(null)

    const handlersRef = useRef(handlers)
    handlersRef.current = handlers

    useEffect(() => {
        const connection = new HubConnectionBuilder()
            .withUrl(HUB_URL)
            .withAutomaticReconnect()
            .build()

        connectionRef.current = connection

        connection
            .start()
            .then(() => {
                const { onCreated, onUpdated, onDeleted } = handlersRef.current

                if (onCreated) connection.on("GuestListCreated", onCreated)
                if (onUpdated) connection.on("GuestListUpdated", onUpdated)
                if (onDeleted) connection.on("GuestListDeleted", onDeleted)
            })
            .catch((err) => {
                console.error("SignalR connection failed:", err)
            })

        return () => {
            const { onCreated, onUpdated, onDeleted } = handlersRef.current

            if (onCreated) connection.off("GuestListCreated", onCreated)
            if (onUpdated) connection.off("GuestListUpdated", onUpdated)
            if (onDeleted) connection.off("GuestListDeleted", onDeleted)

            connection.stop().catch(console.error)
        }
    }, [])
}
