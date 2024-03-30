export interface ILoginResponseDto {
    token: string;
    user: IUserDto;
}

export interface IUserDto {
    id: number;
    name: string;
    email: string;
    department: string;
    roleId: number;
    roleName: string;
    staffCode: string;
    ldapName: string;
    createDate: string;
    createBy: string;
    ldapLogin: boolean;
}

export interface IRoomDto {
    id: number;
    floorGuid: string;
    floorNum: number;
    buildingGuid: string;
    buildingName: string;
    roomNum: string;
    roomTypeId: number;
    description: string;
    capacity: number;
    facilityListGuid: string;
    
    facilities: FacilityDto[];
    createDate: Date;
    createBy: string;
    status: boolean;
    guid: string;
}

export interface FacilityDto {
    id: number;
    name: string;
    type: string;
    brand: string;
    number: number;
    createDate: Date;
    createBy: string;
    status: boolean;
    guid: string;
}
