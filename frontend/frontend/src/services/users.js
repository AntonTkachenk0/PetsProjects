import axios from "axios"

export const fetchUsers = async () => {
    try {
        var response = await axios.get('http://localhost:5067/User/minimalInfo');
        return response.data;

    }
    catch (e) {
        console.log('Ошибка при загрузке пользователей:');
        throw e;
    }
}

export const createUser = async (user) => {
    try {
        var response = await axios.post('http://localhost:5067/User/', user);
        console.log(response.status);
        console.log(response.data);
        return response.status;

    }
    catch (e) {
        console.log('Ошибка при загрузке пользователей:');
        throw e;
    }
}

export const fetchUser = async (userId) => {
    try {
        var response = await axios.get(
            `http://localhost:5067/User/user?id=${userId}`
        )

        return response.data;
    }
    catch (e) {
        console.log(userId)
        throw e;
    }

}

export const fetchUserPhotoBlob = async (photoId) => {
    try {
        var response = await axios.get(
            `http://localhost:5067/api/s3/image/${photoId}`,
            {
                responseType: 'blob'
            }
        );
        return response.data;
    }
    catch (e) {
        alert('Ошибка получения!')
        throw e;
    }
}

export const postPhoto = async (formData, selectedFile) => {

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

export const getStats = async (userId) => {

    try {

        var response = await axios.get('http://localhost:5067/User/take-sum', {
            params: { userId: userId }
        })
        console.log(userId);
        return response.data;
    } catch (error) {
        console.error(error);
    }

}



export const getHistory = async (userId) => {

    try {

        var response = await axios.get('http://localhost:5067/User/take-history', {
            params: { userId: userId }
        })
        console.log(userId);
        return response.data;
    } catch (error) {
        console.error(error);
    }

}

export const updateUserInfo = async (user) => {

    try {

        const response = await axios.put('http://localhost:5067/User/update-info', user)
          
        if (response.status === 200) {
            console.log('Данные успешно обновлены!');
        }
    } catch (error) {
        console.error(error, user);
    }

}