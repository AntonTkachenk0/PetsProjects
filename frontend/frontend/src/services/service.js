import axios from "axios";

export const createService = async (service) => {
    try{
        var response = await axios.post('http://localhost:5067/service/', service);
        alert('Услуга успешно создана!')
        return response.data;
    }
    catch(e){
        alert('Ошибка создания!')
        throw e;
    }
}


export const fetchService = async (service) => {
    try{
        var response = await axios.get('http://localhost:5067/service/');
        console.log('Услуга успешно получена!')
        return response.data;
    }
    catch(e){
        alert('Ошибка получения!')
        throw e;
    }
}
