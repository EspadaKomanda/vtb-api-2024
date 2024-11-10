"use client";
import Image from "next/image";
import { useState, useRef, useEffect } from "react";
import NavigationComponent from "@/components/navigation_component";
import AuntificationPopup from "@/components/auntification_popup";
import auntificationStore from '@/stores/auntification_store.js';

export default function Profile() {
    const [avatar, setAvatar] = useState('/default.png');
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [isDragging, setIsDragging] = useState(false);
    const fileInputRef = useRef(null);
    
    // Get state and actions from the store
    const isLoginOpen = auntificationStore((state) => state.isLoginOpen);
    const isRegisterOpen = auntificationStore((state) => state.isRegisterOpen);
    const openLogin = auntificationStore((state) => state.openLogin);
    const openRegister = auntificationStore((state) => state.openRegister);
    const isAuthenticated = auntificationStore((state) => state.isAuthenticated); // Get isAuthenticated from the store
    const setIsAuthenticated = auntificationStore((state) => state.setIsAuthenticated); // Get setIsAuthenticated from the store

    const [firstName, setFirstName] = useState("Имя");
    const [lastName, setLastName] = useState("Фамилия");
    const [promocode, setPromocode] = useState("Промокод");
    const [bonuses, setBonuses] = useState("0");

    const fetchProfile = async () => {
        try {
            const fullNameResponse = await fetch('/profile_json_files/fullname.json');
            const fullNameData = await fullNameResponse.json();
            setFirstName(fullNameData.firstName);
            setLastName(fullNameData.lastName);

            const promocodeResponse = await fetch('/profile_json_files/promocode.json');
            const promocodeData = await promocodeResponse.json();
            setPromocode(promocodeData.promocode);

            const bonusesResponse = await fetch('/profile_json_files/bonuses.json');
            const bonusesData = await bonusesResponse.json();
            setBonuses(bonusesData.bonuses);
        } catch (error) {
            console.error('Ошибка при получении данных:', error);
        }
    };

    useEffect(() => {
        fetchProfile();
    }, []);

    const handleAvatarChange = (file) => {
        const reader = new FileReader();
        reader.onloadend = () => {
            setAvatar(reader.result);
            setIsModalOpen(false);
        };
        reader.readAsDataURL(file);
    };

    const handleFileInputClick = () => {
        fileInputRef.current.click();
    };

    const handleDrop = (event) => {
        event.preventDefault();
        setIsDragging(false);
        const file = event.dataTransfer.files[0];
        if (file) {
            handleAvatarChange(file);
        }
    };

    const handleDragOver = (event) => {
        event.preventDefault();
        setIsDragging(true);
    };

    const handleDragLeave = () => {
        setIsDragging(false);
    };

    return (
        <>
            <header className="pb-40 bg-cover bg-center" style={{ backgroundImage: "url('/images/bg_profile.jpg')" }}>
                <NavigationComponent />
            </header>
            {isAuthenticated ? (
                <main className="container p-5 text-white">
                    <div className="flex items-center justify-center">
                        <div className="relative w-60 h-60">
                            <Image 
                                src={avatar} 
                                alt="" 
                                width={240} 
                                height={240} 
                                className="rounded-full bg-custom-gradient cursor-pointer border-4 absolute left-1/2 -top-32 transform" 
                                onClick={() => setIsModalOpen(true)}
                            />
                        </div>
                        <h2 className="text-5xl font-bold flex mt-32 -translate-x-1/2">    
                            {lastName} {firstName}
                        </h2>
                    </div>
                    <p className="text-3xl mt-20">Ваш промокод на скидку <span className="font-bold text-4xl pl-5 text-customColor4">{promocode}</span></p>
                    <p className="text-3xl mt-10">Бонусы <span className="font-bold text-4xl pl-16 text-customColor4">{bonuses} Б</span></p> 
                </main>
            ) : (
                <div className="absolute text-3xl font-semibold flex left-1/2 transform -translate-x-1/2">
                    <button onClick={openLogin} className="bg-custom-bg-blue text-white px-4 p-2 rounded">Вход</button>
                    <button onClick={openRegister} className="bg-custom-bg-blue text-white px-4 p-2 rounded ml-2">Регистрация</button>
                </div>
            )}

            {isModalOpen && (
                <div className="fixed inset-0 flex items-center justify-center bg-black bg-opacity-50">
                    <div className="bg-white p-5 rounded shadow-lg">
                        <h2 className="text-lg font-bold mb-4">Редактирование фотографии</h2>
                        <div 
                            className={`border-2 p-4 text-center mb-4 cursor-pointer ${isDragging ? 'rounded-lg bg-blue-100' : 'rounded-lg border-dashed border-gray-300'}`}
                            onClick={handleFileInputClick}
                            onDrop={handleDrop}
                            onDragOver={handleDragOver}
                            onDragLeave={handleDragLeave}
                        >
                            Перетащите изображение сюда или нажмите, чтобы загрузить
                        </div>
                        <input 
                            type="file" 
                            accept="image/*" 
                            onChange={(e) => handleAvatarChange(e.target.files[0])} 
                            ref={fileInputRef}
                            className="hidden"
                        />
                        <button 
                            className="mt-4 bg-custom-bg-blue text-white px-4 py-2 rounded" 
                            onClick={() => setIsModalOpen(false)}
                        >
                            Закрыть
                        </button>
                    </div>
                </div>
            )}
            {isLoginOpen && <AuntificationPopup type="login" />}
            {isRegisterOpen && <AuntificationPopup type="register" />}
        </>
    );
}

