import axios from "axios";

export const createPet = async (pet) => {
    try{
        var response = await axios.post('https://localhost:7241/Pets/', pet);
        alert('Питомец добавлен успешно!')
        return response.data;
    }
    catch(e){
        alert('Ошибка создания!')
        throw e;
    }
}

export const fetchPets = async (pet) => {
    try{
        var response = await axios.get('https://localhost:7241/Pets/');
        return response.data;
    }
    catch(e){
        alert('Ошибка получения!')
        throw e;
    }
}