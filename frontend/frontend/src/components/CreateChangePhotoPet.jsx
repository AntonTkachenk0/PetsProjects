import axios from "axios";
import { useState } from "react";
import { postPhoto } from "../services/users";

export default function CreateChangePhotoPet({ entityId, role }) {

    const [selectedFile, setSelectedFile] = useState(null);

    function handleFileChange(event) {
        const files = event.target.files;

        const oneFile = files[0];
        setSelectedFile(oneFile);
    }

    async function handleSubmit(event) {

        event.preventDefault();

        if (!selectedFile) {
            console.log('Выберите файл!');
            return;
        }

        const formData = new FormData();
        formData.append('file', selectedFile);
        formData.append('PetOrUserId', entityId)
        formData.append('Role', role)

        await postPhoto(formData, selectedFile);



    }
    return (

        <form className='file-upload-form' onSubmit={handleSubmit} >
            <input
                type="file"
                accept=".png,.jpg,.jpeg"
                onChange={handleFileChange}
            />
            
            <button className='submit-btn' type="submit">Сменить</button>
        </form>

    );
}