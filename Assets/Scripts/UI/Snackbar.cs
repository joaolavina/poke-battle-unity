using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class Snackbar : SingletonMonoBehaviour<Snackbar>
{

    [SerializeField] private Animator _animator;
    [SerializeField] private TextMeshProUGUI _messageField;

    protected override void Awake()
    {
        base.Awake();
        // Se estiver nulo (obj criado por código), tenta pegar o componente no mesmo GameObject
        if (_animator == null) _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        if (_animator == null)
        {
            Debug.LogWarning("Snackbar: Animator não atribuído. Desativando snackbar.", this);
            gameObject.SetActive(false);
            return;
        }

        StartCoroutine(PlayAnimationAndDisable());
    }

    public void ShowSnackbar(String text)
    {
        _messageField.text = text;
        gameObject.SetActive(true);
    }

    private IEnumerator PlayAnimationAndDisable()
    {
        float animationLength = _animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animationLength);
        gameObject.SetActive(false);
    }
}
