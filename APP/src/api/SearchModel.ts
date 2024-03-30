export interface SearchRoom{
    BuildingGuid?: string;
    FloorGuid?: string;
    RoomTypeID?: number;
    RoomGuid?: string;
}

export interface SearchUser {
    Department?: string;
    RoleName?: string;
    LDapLogin?: boolean;
}