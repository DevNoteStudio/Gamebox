using System;
using System.Collections.Generic;
using System.Text;
using DevNote;
using UnityEngine;

namespace Gamebox
{

    public class CardsState
    {
        public delegate void OnCardCellChange(int cellIndex);
        public event OnCardCellChange OnCardCellChanged;

        public delegate void OnCardChange(CardType cardType);
        public event OnCardChange OnCardChanged;


        private class CardData
        {
            public CardType cardType;
            public int level;
            public int amount;
            public bool isNew;

            public CardData(CardType cardType, int level, int amount, bool isNew)
            {
                this.cardType = cardType;
                this.level = level;
                this.amount = amount;
                this.isNew = isNew;
            }

            public CardData(string data)
            {
                string[] splitData = data.Split(S.S1);

                cardType = (CardType)int.Parse(splitData[0]);
                level = int.Parse(splitData[1]);
                amount = int.Parse(splitData[2]);
                isNew = splitData[3].FromBinaryToBool();
            }

            public override string ToString() 
                => $"{(int)cardType}{S.S1}{level}{S.S1}{amount}{S.S1}{isNew.ToBinaryString()}";
        }


        private List<CardData> _cardDataList;
        private List<CardType> _cellCards;


        public const int CELLS_AMOUNT = 5;


        public CardsState(string data)
        {
            _cardDataList = new List<CardData>();

            _cellCards = new List<CardType>(capacity: CELLS_AMOUNT);
            for (int i = 0; i < CELLS_AMOUNT; i++)
            {
                if (i == 0) _cellCards.Add(CardType.Empty);
                else _cellCards.Add(CardType.Locked);
            }
                
            string[] splitData = data.Split(S.S3);

            if (splitData.Length == 2)
            {
                string[] splitCellCardsData = splitData[0].Split(S.S1);

                for (int i = 0; i < splitCellCardsData.Length; i++)
                    _cellCards[i] = (CardType)int.Parse(splitCellCardsData[i]);

                string[] splitCardsData = splitData[1].Split(S.S2);

                for (int i = 0; i < splitCardsData.Length; i++)
                {
                    if (splitCardsData[i] != string.Empty)
                        _cardDataList.Add(new CardData(splitCardsData[i]));
                }
            }
            
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            for (int i = 0; i < _cellCards.Count; i++)
            {
                if (i != 0) builder.Append(S.S1);
                builder.Append(((int)_cellCards[i]).ToString());
            }

            builder.Append(S.S3);

            for (int i = 0; i < _cardDataList.Count; i++)
            {
                if (i != 0) builder.Append(S.S2);
                builder.Append(_cardDataList[i].ToString());
            }

            return builder.ToString();
        }


        private CardData GetCardData(CardType cardType)
        {
            var data = _cardDataList.Find(data => data.cardType == cardType);

            if (data == null)
            {
                data = new CardData(cardType, level: 0, amount: 0, isNew: true);
                _cardDataList.Add(data);
            }  

            return data;
        }

        public bool IsNew(CardType cardType) => GetCardData(cardType).isNew;

        public void SetCardAsViewed(CardType cardType) => GetCardData(cardType).isNew = false;

        public CardType GetCellCard(int cellIndex) => _cellCards[cellIndex];

        public void SetCardToCell(int cellIndex, CardType cardType)
        {
            _cellCards[cellIndex] = cardType;
            OnCardCellChanged?.Invoke(cellIndex);
        }


        public bool IsActive(CardType cardType) => _cellCards.Contains(cardType);

        public bool Has(CardType cardType) => GetCardData(cardType).amount > 0;

        public int GetLevel(CardType cardType) => GetCardData(cardType).level;

        public void IncreaseLevel(CardType cardType)
        {
            GetCardData(cardType).level++;
            OnCardChanged?.Invoke(cardType);
        }

        public int GetAmountOnCurrentLevel(CardType cardType)
        {
            int level = GetCardData(cardType).level;
            int amount = GetCardData(cardType).amount;
            int passedCards = 1;

            for (int passedLevel = 1; passedLevel < level; passedLevel++)
                passedCards += (int)Mathf.Pow(2, passedLevel - 1);

            return amount - passedCards;
        }

        public int GetRequiredCardsOnCurrentLevel(CardType cardType) 
            => (int)Mathf.Pow(2, GetCardData(cardType).level - 1);


        public void IncreaseAmount(CardType cardType, int value)
        {
            GetCardData(cardType).amount += value;
            OnCardChanged?.Invoke(cardType);
        }



    }
}
