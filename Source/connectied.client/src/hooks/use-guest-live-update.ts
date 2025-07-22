import { useEffect, useRef } from "react"
import { HubConnectionBuilder, type HubConnection } from "@microsoft/signalr"

const HUB_URL = "/hubs/guest"

export function useGuestLiveUpdate(handlers: {
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

                if (onCreated) connection.on("GuestCreated", onCreated)
                if (onUpdated) connection.on("GuestUpdated", onUpdated)
                if (onDeleted) connection.on("GuestDeleted", onDeleted)
            })
            .catch((err) => {
                console.error("SignalR connection failed:", err)
            })

        return () => {
            const { onCreated, onUpdated, onDeleted } = handlersRef.current

            if (onCreated) connection.off("GuestCreated", onCreated)
            if (onUpdated) connection.off("GuestUpdated", onUpdated)
            if (onDeleted) connection.off("GuestDeleted", onDeleted)

            connection.stop().catch(console.error)
        }
    }, [])
}
