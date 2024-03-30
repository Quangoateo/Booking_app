import axios from 'axios';
import * as model from  './Models';
import * as Seachmodel from './SearchModel';
const API_URL="http://localhost:58";

const instance = axios.create({
    baseURL: API_URL,
    // timeout: 1000,
    headers: {
        'Content-Type': 'application/json',
    }
});

export function POST_LDAP_AUTH(object : model.ILogin) {
    return instance.post('api/LdapAuth/Login', object);
}
export function ADD_FACILITY(object : model.IFacility) {
    return instance.post('api/Facility/Add', object);
}
export function ADD_ROOM(object : model.IRoom) {
    return instance.post('api/Room/Add', object);
}
export function SEARCH_ROOM(object: Seachmodel.SearchRoom) {
    
    const request ={
        params: object
    }
    return instance.get('api/Room/SearchRoom', request);
}
export function SEARCH_USER(object: Seachmodel.SearchUser) {

    const request ={
        params: object
    }
    return instance.get('api/User/SearchUser', request);
}
export function REGISTER(object: model.IUserRegister){
    return instance.post('api/User/Register', object);
}
export function LOGIN_NON_LDAP(object: model.ILogin){
    return instance.post('api/User/Login', object);
}