import axios from "axios";

export const createServiceJournal = async (journal) => {
    try{
        var response = await axios.post('http://localhost:5067/serviceJournal/', journal);
        alert('Вы успешно записались!')
        return response.data;
    }
    catch(e){
        alert('Ошибка записи!')
        throw e;
    }
}