import NavigationComponent from "@/components/navigation_component";
import WishListComponent from "@/components/wish_list_component";

export default function WishList() {
    return (
        <div>
            <header>
                <NavigationComponent/>
            </header>
            <main className="container p-5">
                <WishListComponent/>
            </main>
        </div>
    );
}