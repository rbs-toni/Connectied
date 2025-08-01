﻿import { useEffect, useState } from "react"
import { useBreadcrumb } from "@/hooks/use-breadcrumb"
import type { GuestStats } from "@/types"
import { client } from "@/api"
import {
    Card,
    CardDescription,
    CardHeader,
    CardTitle,
    CardContent
} from "@/components/ui/card"
import { Pie, PieChart, Label, Cell } from "recharts"
import {
    ChartContainer,
    ChartLegend,
    ChartLegendContent,
    ChartTooltip,
    ChartTooltipContent,
    type ChartConfig
} from "@/components/ui/chart"
import { useGuestLiveUpdate } from "@/hooks/use-guest-live-update"

const pieColors = ["#4f46e5", "#22c55e", "#f97316"]

const chartConfig = {
    event1: { label: "Event 1", color: pieColors[0] },
    event2: { label: "Event 2", color: pieColors[1] },
} satisfies ChartConfig

const buildChartData = (event1: number, event2: number) => [
    { name: "Event 1", value: event1 },
    { name: "Event 2", value: event2 },
]

export default function DashboardPage() {
    const [guestStats, setGuestStats] = useState<GuestStats>()
    const [isLoading, setIsLoading] = useState(true)
    const { setItems: setBreadcrumbItems } = useBreadcrumb()

    const loadInitialGuestStats = async () => {
        try {
            const data = await client.getGuestStats()
            setGuestStats(data)
        } catch (error) {
            console.error("[Dashboard] Initial fetch failed:", error)
        } finally {
            setIsLoading(false)
        }
    }

    const updateGuestStats = async (source: string) => {
        console.debug(`[Dashboard] 🔄 Live update triggered by: ${source}`)
        try {
            const data = await client.getGuestStats()
            setGuestStats(data)
            console.debug("[Dashboard] ✅ Live update success:", data)
        } catch (error) {
            console.error("[Dashboard] ❌ Live update failed:", error)
        }
    }

    useEffect(() => {
        loadInitialGuestStats()
    }, [])


    useEffect(() => {
        setBreadcrumbItems([{ title: "Dashboard" }])
    }, [])

    useGuestLiveUpdate({
        onCreated: updateGuestStats,
        onUpdated: updateGuestStats,
        onDeleted: updateGuestStats,
    })

    if (isLoading) return <p>Loading guest statistics...</p>
    if (!guestStats) return null

    const notComing = guestStats.quota - guestStats.attendance
    const undanganChartData = [
        { name: "Hadir", value: guestStats.attendance },
        { name: "Tidak Hadir", value: notComing },
    ]

    return (
        <div className="grid gap-4 sm:grid-cols-2 lg:grid-cols-1 xl:grid-cols-2">
            <Card className="@container/card">
                <CardHeader>
                    <CardDescription>Undangan</CardDescription>
                    <CardTitle className="text-2xl font-semibold tabular-nums @[250px]/card:text-3xl">
                        {guestStats.quota.toLocaleString()}
                    </CardTitle>
                </CardHeader>
                <CardContent>
                    <ChartContainer config={{ hadir: { label: "Hadir", color: pieColors[0] }, tidak: { label: "Tidak Hadir", color: pieColors[2] } }} className="mx-auto aspect-square max-h-[250px]">
                        <PieChart>
                            <ChartTooltip cursor={false} content={<ChartTooltipContent hideLabel />} />
                            <Pie
                                data={undanganChartData}
                                dataKey="value"
                                nameKey="name"
                                innerRadius={60}
                                strokeWidth={5}
                            >
                                {undanganChartData.map((entry, index) => (
                                    <Cell key={`cell-undangan-${index}`} fill={pieColors[index % pieColors.length]} />
                                ))}
                                <Label
                                    content={({ viewBox }) => {
                                        if (viewBox && "cx" in viewBox && "cy" in viewBox) {
                                            return (
                                                <text
                                                    x={viewBox.cx}
                                                    y={viewBox.cy}
                                                    textAnchor="middle"
                                                    dominantBaseline="middle"
                                                >
                                                    <tspan
                                                        x={viewBox.cx}
                                                        y={viewBox.cy}
                                                        className="fill-foreground text-3xl font-bold"
                                                    >
                                                        {guestStats.quota.toLocaleString()}
                                                    </tspan>
                                                    <tspan
                                                        x={viewBox.cx}
                                                        y={(viewBox.cy || 0) + 24}
                                                        className="fill-muted-foreground"
                                                    >
                                                        Diundang
                                                    </tspan>
                                                </text>
                                            )
                                        }
                                    }}
                                />
                            </Pie>
                            <ChartLegend
                                content={<ChartLegendContent nameKey="name" />}
                                className="-translate-y-2 flex-wrap gap-2 *:basis-1/4 *:justify-center"
                            />
                        </PieChart>
                    </ChartContainer>
                </CardContent>
            </Card>

            {[
                {
                    title: "Attendance",
                    data: buildChartData(guestStats.event1Attendance, guestStats.event2Attendance),
                    total: guestStats.attendance,
                    unit: "Attendees",
                },
                {
                    title: "Angpao",
                    data: buildChartData(guestStats.event1Angpao, guestStats.event2Angpao),
                    total: guestStats.angpao,
                    unit: "Total",
                },
                {
                    title: "Gift",
                    data: buildChartData(guestStats.event1Gift, guestStats.event2Gift),
                    total: guestStats.gift,
                    unit: "Total",
                },
                {
                    title: "Souvenir",
                    data: buildChartData(guestStats.event1Souvenir, guestStats.event2Souvenir),
                    total: guestStats.souvenir,
                    unit: "Total",
                },
                {
                    title: "Quota",
                    data: buildChartData(guestStats.event1Quota, guestStats.event2Quota),
                    total: guestStats.quota,
                    unit: "Quota",
                },
            ].map(({ title, data, total, unit }) => (
                <Card key={title} className="@container/card">
                    <CardHeader>
                        <CardDescription>{title}</CardDescription>
                        <CardTitle className="text-2xl font-semibold tabular-nums @[250px]/card:text-3xl">
                            {total.toLocaleString()}
                        </CardTitle>
                    </CardHeader>
                    <CardContent>
                        <ChartContainer config={chartConfig} className="mx-auto aspect-square max-h-[250px]">
                            <PieChart>
                                <ChartTooltip cursor={false} content={<ChartTooltipContent  />} />
                                <Pie
                                    data={data}
                                    dataKey="value"
                                    nameKey="name"
                                    innerRadius={60}
                                    strokeWidth={5}
                                >
                                    {data.map((entry, index) => (
                                        <Cell key={`cell-${index}`} fill={pieColors[index % pieColors.length]} />
                                    ))}
                                    <Label
                                        content={({ viewBox }) => {
                                            if (viewBox && "cx" in viewBox && "cy" in viewBox) {
                                                return (
                                                    <text
                                                        x={viewBox.cx}
                                                        y={viewBox.cy}
                                                        textAnchor="middle"
                                                        dominantBaseline="middle"
                                                    >
                                                        <tspan
                                                            x={viewBox.cx}
                                                            y={viewBox.cy}
                                                            className="fill-foreground text-3xl font-bold"
                                                        >
                                                            {total.toLocaleString()}
                                                        </tspan>
                                                        <tspan
                                                            x={viewBox.cx}
                                                            y={(viewBox.cy || 0) + 24}
                                                            className="fill-muted-foreground"
                                                        >
                                                            {unit}
                                                        </tspan>
                                                    </text>
                                                )
                                            }
                                        }}
                                    />
                                </Pie>
                                <ChartLegend
                                    content={<ChartLegendContent nameKey="name" />}
                                    className="-translate-y-2 flex-wrap gap-2 *:basis-1/4 *:justify-center"
                                />
                            </PieChart>
                        </ChartContainer>
                    </CardContent>
                </Card>
            ))}
        </div>
    )
}
