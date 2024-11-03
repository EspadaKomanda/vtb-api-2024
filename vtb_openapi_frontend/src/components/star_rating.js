import Image from "next/image";
import * as img from '../assets/images.js'


const StarRating = ({ rating, setRating }) => {
    const stars = [1, 2, 3, 4, 5];
    const filledStar = img.star_active;
    const emptyStar = img.star;

    return (
        <div className="flex">
            {stars.map((star) => (
                <Image
                    key={star}
                    src={star <= rating ? filledStar : emptyStar}
                    alt={`${star} star`}
                    className="cursor-pointer w-6 h-6 mr-2"
                    onClick={() => setRating(star)}
                />
            ))}
        </div>
    );
};

export default StarRating;

