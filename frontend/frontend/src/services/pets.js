import axios from "axios";

export const createPet = async (pet) => {
    try{
        var response = await axios.post('http://localhost:5067/api/Pets/', pet);
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
        var response = await axios.get('http://localhost:5067/api/Pets/');
        return response.data;
    }
    catch(e){
       console.log('Ошибка получения!')
        throw e;
    }
}

export const fetchPhotoBlob = async (photoId) => {
    try{
        var response = await axios.get(
            `http://localhost:5067/api/s3/image/${photoId}/`,
            {
                responseType: 'blob'
            }
        );
        return response.data;
    }
    catch(e){
        console.log('Ошибка получения!')
        throw e;
    }
}

export const postPhotoPet = async (formData, selectedFile) => {

    try {

        const response = await axios.post('http://localhost:5067/api/s3/image', formData,
            {
                headers:
                    { 'Content-Type': 'multipart/form-data' }
            });

        if (response.status === 200) {
            console.log('Фото успешно добавлено!');
        }
    } catch (error) {
        console.error(error, selectedFile);
    }

}