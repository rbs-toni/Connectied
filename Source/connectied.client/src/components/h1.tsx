"use client"

import * as React from "react"
import { cn } from "@/lib/utils"

function TypographyH1({
    className,
    children,
    ...props
}: React.HTMLAttributes<HTMLHeadingElement>) {
    return (
        <h1
            className={cn(
                "scroll-m-20 text-4xl font-extrabold tracking-tight balance",
                className
            )}
            {...props}
        >
            {children}
        </h1>
    )
}

export { TypographyH1 }
