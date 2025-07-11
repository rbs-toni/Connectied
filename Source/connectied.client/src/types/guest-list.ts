export interface GuestList {
    id: string
    name: string
    group?: string
    event1Quota: number
    event2Quota: number
    event1Rsvp: number
    event2Rsvp: number
    event1Attend: number
    event2Attend: number
    event2AngpaoCount: number
    event2GiftCount: number
    event2Souvenir: number
    notes?: string
}