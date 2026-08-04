import { useState } from 'react'
import './css/petForm.css';
import './css/userForm.css';
import './css/index.css';
import './css/mainPage.css';

import { createUser, fetchUsers } from './services/users';
import { useEffect } from 'react';
import CreateUserForm from './components/CreateUserForm';
import CreateMainPage from './components/CreateMainPage';
import CreateServiceForm from './components/CreateServiceForm';
import CreatePetForm from './components/CreatePetForm';
import { Route, Routes } from 'react-router-dom';
import Navbar from './components/Navbar';
import { createPet } from './services/pets';
import { createService } from './services/service';

function App() {

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
  return (
    <>
    
    <Navbar />
    <Routes>
          <Route path="/" element={<CreateMainPage/>} />
          <Route path="/user" element={<CreateUserForm onCreate={handleCreateUser}/>} />
          <Route path="/pet" element={<CreatePetForm onCreate={handleCreatePet}/>} />
          <Route path="/service" element={<CreateServiceForm onCreate={handleCreateService}/>} />
        </Routes>
    </>
  )
}

export default App
