using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryMenuController : MonoBehaviour
{
    [SerializeField] private RectTransform _inventoryScroll;
    [SerializeField] private Image _leftArrow;
    [SerializeField] private Image _rightArrow;
    [SerializeField] private Color _arrowColour = new Color(0.3125f, 0.3125f, 0.3125f, 1);

    private int _pageNumber;
    [SerializeField] private int _totalPages = 6;
    [SerializeField] private float _pageWidth = 512.0f;
    [SerializeField] private float _scrollSpeed = 1024.0f;
    private float _scrollPosition;
    private float _smoothDampCurrentVelocity;

    private void OnEnable()
    {
        //init fields
        _pageNumber = 3;//TODO: set based on current level///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        _inventoryScroll.anchoredPosition = new Vector2(_pageNumber * -_pageWidth, _inventoryScroll.anchoredPosition.y);
        _scrollPosition = _inventoryScroll.anchoredPosition.x;
    }

    public void PageRight()
    {
        if (_pageNumber < _totalPages - 1)
        {
            _pageNumber++;
            _scrollPosition -= _pageWidth;
            StopAllCoroutines();
            StartCoroutine(ScrollPage());
            UpdateArrows();
        }
    }

    public void PageLeft()
    {
        if (_pageNumber > 0)
        {
            _pageNumber--;
            _scrollPosition += _pageWidth;
            StopAllCoroutines();
            StartCoroutine(ScrollPage());
            UpdateArrows();
        }
    }

    private IEnumerator ScrollPage()
    {
        while (_inventoryScroll.anchoredPosition.x != _scrollPosition)
        {
            if (Mathf.Abs(_inventoryScroll.anchoredPosition.x - _scrollPosition) < 1.0f)
            {
                //snap to position if less than a pixel
                _inventoryScroll.anchoredPosition = new Vector2(_scrollPosition, _inventoryScroll.anchoredPosition.y);
            }
            else
            {
                //scroll page
                _inventoryScroll.anchoredPosition = new Vector2(Mathf.SmoothDamp(_inventoryScroll.anchoredPosition.x, _scrollPosition, ref _smoothDampCurrentVelocity, _pageWidth / _scrollSpeed, _scrollSpeed, Time.unscaledDeltaTime), _inventoryScroll.anchoredPosition.y);
            }

            yield return null;
        }
    }

    private void UpdateArrows()
    {
        if (_pageNumber == 0)
        {
            _leftArrow.color = Color.clear;
            _rightArrow.color = _arrowColour;
        }
        else if (_pageNumber == _totalPages - 1)
        {
            _leftArrow.color = _arrowColour;
            _rightArrow.color = Color.clear;
        }
        else
        {
            _leftArrow.color = _arrowColour;
            _rightArrow.color = _arrowColour;
        }
    }
}
