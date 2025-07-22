import { useState } from "react"
import { Command, CommandGroup, CommandItem, CommandInput } from "@/components/ui/command"
import { Popover, PopoverContent, PopoverTrigger } from "@/components/ui/popover"
import { Button } from "@/components/ui/button"
import { Checkbox } from "./checkbox"

export function MultiSelect({ selected, options, onChange, placeholder }: {
    selected: string[]
    options: { value: string, label: string }[]
    onChange: (values: string[]) => void
    placeholder?: string
}) {
    const [open, setOpen] = useState(false)

    const toggleValue = (value: string) => {
        if (selected.includes(value)) {
            onChange(selected.filter(v => v !== value))
        } else {
            onChange([...selected, value])
        }
    }

    return (
        <Popover open={open} onOpenChange={setOpen}>
            <PopoverTrigger asChild>
                <Button variant="outline" className="w-full justify-between">
                    {selected.length > 0 ? `${selected.length} selected` : placeholder || "Select..."}
                </Button>
            </PopoverTrigger>
            <PopoverContent className="w-full p-0">
                <Command>
                    <CommandInput placeholder="Search..." />
                    <CommandGroup>
                        {options.map((opt) => (
                            <CommandItem
                                key={opt.value}
                                onSelect={() => toggleValue(opt.value)}
                                className="cursor-pointer"
                            >
                                <Checkbox
                                    checked={selected.includes(opt.value)}
                                    onCheckedChange={() => toggleValue(opt.value)}
                                />
                                <span className="ml-2">{opt.label}</span>
                            </CommandItem>
                        ))}
                    </CommandGroup>
                </Command>
            </PopoverContent>
        </Popover>
    )
}
