import { useEffect, useState } from "react"
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
import { ChartContainer, ChartTooltip, ChartTooltipContent, type ChartConfig } from "@/components/ui/chart"

const pieColors = ["#4f46e5", "#22c55e"]

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

    useEffect(() => {
        const loadGuestStats = async () => {
            try {
                const data = await client.getGuestStats()
                setGuestStats(data)
            } catch (error) {
                console.error("Failed to fetch guest stats", error)
            } finally {
                setIsLoading(false)
            }
        }
        loadGuestStats()
    }, [])

    useEffect(() => {
        setBreadcrumbItems([{ title: "Dashboard" }])
    }, [setBreadcrumbItems])

    if (isLoading) return <p>Loading guest statistics...</p>

    if (!guestStats) return null

    return (
        <div className="grid gap-4 sm:grid-cols-2 lg:grid-cols-1 xl:grid-cols-2">
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
            ].map(({ title, data, total, unit }, i) => (
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
                                <ChartTooltip cursor={false} content={<ChartTooltipContent hideLabel />} />
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
                            </PieChart>
                        </ChartContainer>
                    </CardContent>
                </Card>
            ))}
        </div>
    )
}
