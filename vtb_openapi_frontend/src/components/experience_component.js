import Link from 'next/link';
import StarRating from './star_rating.js';
import LikeButton from "./like_component";

const ExperienceItem = ({ experience }) => {
    return (
        <>
            <li className="relative h-96 flex flex-col justify-end rounded">
                <Link href={`/experience/${experience.id}`} passHref>
                    <div
                        className="absolute inset-0 bg-cover  bg-center rounded cursor-pointer"
                        style={{ backgroundImage: `url(${experience.image})` }}
                    >
                        <div className="absolute inset-0 rounded"></div>
                    </div>
                    <div className="relative min-h-1/3 h-auto p-4 bg-custom-blur bg-opacity-60 backdrop-blur-sm rounded"> 
                        <h4 className="text-white text-lg font-bold">{experience.name}</h4>
                        <p className="text-gray-300">Описание: {experience.description}</p>
                        <p className="text-gray-300">Цена: {experience.price} ₽</p>
                        <div className='flex'>
                            <p className="text-gray-300 mr-2">Рейтинг:</p> 
                            <StarRating rating={experience.rating} editable={false}/>
                        </div>
                    </div>
                </Link>
            </li>
            <div className='mt-3 mb-10 flex justify-end text-white text-3xl font-semibold'>
                <LikeButton 
                    initialLiked={experience.like}
                    initialLikeCount={experience.likes}
                />
            </div>
        </>
    );
    
};

export default ExperienceItem;
