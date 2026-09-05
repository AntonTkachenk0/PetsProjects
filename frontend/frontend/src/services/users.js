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

export const fetchUser = async () => {
    try {
        var response = await axios.get(
            `http://localhost:5067/User/user`, {withCredentials: true});

        return response.data;
    }
    catch (e) {
        console.log("Ошибка получения пользователя")
        throw e;
    }

}

export const fetchUserPhotoBlob = async (photoId) => {
    try {
        var response = await axios.get(
            `http://localhost:5067/api/s3/image/${photoId}`,
            {
                responseType: 'blob',
                withCredentials: true
            },
          
        );
        return response.data;
    }
    catch (e) {
        console.log('Ошибка получения!')
        throw e;
    }
}

export const postPhoto = async (formData, selectedFile) => {

    try {
        console.log("я тут")
        const response = await axios.post('http://localhost:5067/api/s3/image', formData,
            {
                headers:
                    { 'Content-Type': 'multipart/form-data' },
                    withCredentials: true
            });
        console.log("я тут2")
        if (response.status === 200) {
            console.log('Фото успешно добавлено!');
        }
    } catch (error) {
        console.error(error, selectedFile);
    }

}

export const getStats = async () => {

    try {

        var response = await axios.get('http://localhost:5067/User/take-sum', {withCredentials: true})
        return response.data;
    } catch (error) {
        console.error(error);
    }

}



export const getHistory = async () => {

    try {

        var response = await axios.get('http://localhost:5067/User/take-history', {withCredentials: true});
        return response.data;
    } catch (error) {
        console.error(error);
    }

}

export const updateUserInfo = async (user) => {

    try {

        console.log(userId);
        const response = await axios.put('http://localhost:5067/User/update-info', user, {withCredentials: true} )
          
        if (response.status === 200) {
            console.log('Данные успешно обновлены!');
        }
    } catch (error) {
        console.error(error, user);
    }

}


export const loginUser = async (user) => {

    try {
        const response = await axios.post('http://localhost:5067/User/login', user, { 
             withCredentials: true
            }); 
          
        if (response.status === 200) {
            console.log('Пользователь вошел в систему!');
        }
    } catch (error) {
        console.error(error, user);
    }

}


export const logoutUser = async () => {

    try {
        const response = await axios.post('http://localhost:5067/User/logout', {}, { 
             withCredentials: true
            });
          
        if (response.status === 200) {
            console.log('Пользователь вышел из системы!');
        }
    } catch (error) {
        console.error(error);
    }

}
