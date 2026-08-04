import axios from "axios";

export const createService = async (service) => {
    try{
        var response = await axios.post('https://localhost:7241/service/', service);
        alert('Услуга успешно создана!')
        return response.data;
    }
    catch(e){
        alert('Ошибка создания!')
        throw e;
    }
}
