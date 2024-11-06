"use client"
import Link from 'next/link';

const ExperienceItem = ({ experience }) => {
    return (
        <li className="relative h-96 flex flex-col justify-end rounded mb-10">
            <Link href={`/experience/${experience.id}`} passHref>
                <div
                    className="absolute inset-0 bg-cover bg-center rounded cursor-pointer"
                    style={{ backgroundImage: `url(${experience.image})` }}
                >
                    <div className="absolute inset-0 bg-customColor1 opacity-50 rounded"></div>
                </div>
                <div className="relative min-h-1/3 h-auto p-4 backdrop-blur-md rounded"> 
                    <h4 className="text-white text-lg font-bold">{experience.name}</h4>
                    <p className="text-gray-300">Описание: {experience.description}</p>
                    <p className="text-gray-300">Цена: {experience.price} ₽</p>
                    <p className="text-gray-300">Рейтинг: {experience.rating}</p>
                </div>
            </Link>
        </li>
    );
};

export default ExperienceItem;
