import React from "react";
import Card from "./Card";

const CardList = () => {
  return (
    <div>
      <Card
        image="https://th.bing.com/th/id/OIP.y4p6mRVTwf2k7HR7p8f3uQHaE6?w=5900&h=3920&rs=1&pid=ImgDetMain"
        title="Burger"
        price={100}
        description="This is a special Burger."
      />

      <Card
        image="https://www.pixelstalk.net/wp-content/uploads/2016/08/Free-Food-Images-Download.jpg"
        title="Pizza"
        price={60}
        description="You can customize the ingredients as your wish."
      />

      <Card
        image="https://th.bing.com/th/id/OIP.ITZKC3C827ofrq4DAQ0uuwHaEK?w=1920&h=1080&rs=1&pid=ImgDetMain"
        title="Healthy Meal"
        price={75}
      />
    </div>
  );
};

export default CardList;
