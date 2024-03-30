
export interface ILogin {
    username: string;
    password: string;
    // rememberMe: boolean;
}

export interface IRoom{
    floorGuid : string;
    roomNum : string;
    roomTypeId : number;
    description: string;
    capacity: number;
    createDate: Date;
    createBy: string;
}

export interface IFacility
{
    name: string;
    type: string;
    brand: string;
    createDate: Date;
    createBy: number;
}

export interface IFloor
{
    floorNum: number;
    building: string;
    createDate: Date;
    createBy: string;
}

export interface IBuilding
{
    name: string;
    block: string;
    locationX: number;
    locationY: number;
    createDate: Date;
    createBy: string;
}

export interface IBooking
{
    userGuid: string;
    roomGuid: string;
    bookingDate: Date;
    bookingTimeS: string;
    bookingTimeE: string;
    orderDate: Date;
    startDate: Date;
    endDate: Date;
    description: string;
    comment: string;
    bookingStatus: number;
    createDate: Date;
    createBy: number;
}
export interface IUserRegister{
    name : string;
    email : string;
    department : string;
    roleId : number;
    password: string;
    createDate: Date;
    createBy: number;
}