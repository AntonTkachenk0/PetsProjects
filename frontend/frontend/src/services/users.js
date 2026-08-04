import axios from "axios"

export const fetchUsers = async () => {
    try{
        var response = await axios.get('https://localhost:7241/User/minimalInfo');
        return response.data;
       
    }
    catch (e){
        console.log('Ошибка при загрузке пользователей:');
        throw e;
    }
}

export const createUser = async (user) => {
    try{
        var response = await axios.post('https://localhost:7241/User/', user);
        console.log(response.status);
        return response.status;
       
    }
    catch (e){
        console.log('Ошибка при загрузке пользователей:');
        throw e;
    }
}
