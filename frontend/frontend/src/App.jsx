import { useState } from 'react'
import './css/petForm.css';
import './css/userForm.css';
import './css/index.css';
import './css/mainPage.css';
import './css/profilePage.css';
import './css/serviceJournal.css';
import '@fontsource-variable/inter-tight';
import { createUser, fetchUser, fetchUserPhotoBlob, fetchUsers, loginUser } from './services/users';
import { useEffect } from 'react';
import CreateUserForm from './components/pages/CreateUserForm';
import CreateMainPage from './components/pages/CreateMainPage';
import CreateServiceForm from './components/pages/CreateServiceForm';
import CreateServiceJournalForm from './components/pages/CreateServiceJournalForm';
import CreatePetForm from './components/pages/CreatePetForm';
import CreateProfilePage from './components/pages/CreateProfilePage';
import { Route, Routes } from 'react-router-dom';
import Navbar from './components/pages/Navbar';
import { createPet } from './services/pets';
import { createService } from './services/service';
import { createServiceJournal } from './services/serviceJournal';
import CreateLoginPage from './components/pages/CreateLoginPage';

function App() {

  const [photoUrl, setPhotoUrl] = useState(null);
  const [user, setUser] = useState(null);

  useEffect(() => {
    const loadPhoto = async () => {
      const user = await fetchUser();
      console.log(user);
      setUser(user);
      const blob = await fetchUserPhotoBlob(user.photoS3Key);

      const url = URL.createObjectURL(blob);
      setPhotoUrl(url);
      console.log('фото получено!');

      return () => {
        if (photoUrl) {
          URL.revokeObjectURL(photoUrl);
          console.log('Память освобождена');
        }
      };
    }
    loadPhoto();

    return () => {
      if (photoUrl) {
        URL.revokeObjectURL(photoUrl);
      }
    };

  }, [])

  const handleCreateUser = (user) => {

    createUser(user);
    console.log(user);
  }


  const handleCreatePet = (pet) => {

    createPet(pet);
    console.log(pet);
  }

  const handleGetPet = (pet) => {
    fetchPets(pet);
    console.log(pet);
  }

  const handleCreateService = (service) => {

    createService(service);
    console.log(service);
  }

  const handleCreateServiceJournal = (serviceJournal) => {

    createServiceJournal(serviceJournal);
    console.log(serviceJournal);
  }

  const handleLoginUser = (user) => {

    loginUser(user);
    console.log("Успешная авторизация");
  }



  return (
    <>

      <Navbar photoUrl={photoUrl} />
      <Routes>
        <Route path="/" element={<CreateMainPage />} />
        <Route path="/user" element={<CreateUserForm onCreate={handleCreateUser} />} />
        <Route path="/pet" element={<CreatePetForm onCreate={handleCreatePet} />} />
        <Route path="/service" element={<CreateServiceForm onCreate={handleCreateService} />} />
        <Route path="/serviceJournal" element={<CreateServiceJournalForm onCreate={handleCreateServiceJournal} />} />
        <Route path="/profile" element={<CreateProfilePage user={user} photoUrl={photoUrl} onCreate={handleCreateService} />} />
        <Route path="/login" element={<CreateLoginPage onCreate={handleLoginUser} />} />
      </Routes>
    </>
  )
}

export default App
