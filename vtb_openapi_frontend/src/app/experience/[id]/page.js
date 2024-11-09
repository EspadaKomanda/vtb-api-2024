"use client";

import { useEffect, useState } from 'react';
import { use } from 'react';
import Image from 'next/image';
import { motion } from 'framer-motion';
import StarRating from '@/components/star_rating.js';

const ExperienceDetailPage = ({ params }) => {
    const { id } = use(params);
    const [experience, setExperience] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        const fetchExperience = async () => {
            try {
                setLoading(true);
                const response = await fetch(`/serch_json_files/${id}_tour.json`);
                if (!response.ok) {
                    throw new Error('Ошибка при загрузке данных');
                }
                const data = await response.json();
                setExperience(data);
            } catch (err) {
                setError(err.message);
            } finally {
                setLoading(false);
            }
        };

        fetchExperience();
    }, [id]);

    if (loading) {
        return <div className='text-3xl text-white'>Загрузка...</div>;
    }

    if (error) {
        return <div className='text-3xl text-white'>Ошибка: {error}</div>;
    }

    if (!experience) {
        return <div className='text-3xl text-white'>Что-то потерялось..</div>;
    }

    return (
        <div className="relative rounded pt-10">
            <motion.div
                className="relative p-4 backdrop-blur-md rounded container "
                initial={{ opacity: 0, y: -20 }}
                animate={{ opacity: 1, y: 0 }}
                transition={{ duration: 0.3, ease: 'easeInOut' }}
            >
                <h1 className="text-white text-3xl font-bold mb-5">{experience.name}</h1>
                <Image src={experience.image} alt={experience.name} width={1000} height={1000} className='w-full h-auto' />
                <p className="text-gray-300 mt-10 text-3xl"><strong>Описание:</strong> {experience.description}</p>
                <p className="text-gray-300 mt-4 text-3xl"><strong>Адрес:</strong> {experience.description}</p>
                <div className='flex mt-4'>
                    <p className="text-gray-300  mr-5 text-3xl"><strong>Рейтинг:</strong> </p>
                    <div className="flex items-center">
                        <StarRating rating={experience.rating} editable={false}/>
                    </div>
                </div>
                <p className="text-gray-300 mt-2 text-3xl"><strong>Стоимость:</strong> от {experience.price} ₽</p>
            </motion.div>
        </div>
    );
};

export default ExperienceDetailPage;
